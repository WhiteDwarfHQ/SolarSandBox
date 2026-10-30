const startupDbName = 'solar-sandbox-startup';
const startupDbVersion = 1;
const projectStoreName = 'projects';
const appStoreName = 'app';
const lastActiveProjectIdKey = 'lastActiveProjectId';

window.solarSandboxStartupStorage = {
    initialize,
    getLastActiveProject,
    getLastActiveProjectDocument,
    createOrUpdateProject,
    setLastActiveProject
};

async function initialize() {
    await openDatabase();
}

async function getLastActiveProject() {
    const document = await getLastActiveProjectDocument();
    return document ? document.project : null;
}

async function getLastActiveProjectDocument() {
    const db = await openDatabase();

    const appTx = db.transaction(appStoreName, 'readonly');
    const appStore = appTx.objectStore(appStoreName);
    const appEntry = await requestToPromise(appStore.get(lastActiveProjectIdKey));
    await transactionToPromise(appTx);

    if (!appEntry || !appEntry.value) {
        return null;
    }

    const projectTx = db.transaction(projectStoreName, 'readonly');
    const projectStore = projectTx.objectStore(projectStoreName);
    const storedProject = await requestToPromise(projectStore.get(appEntry.value));
    await transactionToPromise(projectTx);

    if (!storedProject) {
        return null;
    }

    if (isProjectDocument(storedProject)) {
        return storedProject;
    }

    const migrated = {
        id: storedProject.id,
        schemaVersion: 1,
        appVersion: '0.1.0',
        project: storedProject,
        nodes: []
    };

    await createOrUpdateProject(migrated);

    return migrated;
}

async function createOrUpdateProject(projectDocument) {
    const normalized = normalizeProjectDocument(projectDocument);

    const db = await openDatabase();
    const tx = db.transaction(projectStoreName, 'readwrite');
    const store = tx.objectStore(projectStoreName);

    store.put(normalized);

    await transactionToPromise(tx);
}

async function setLastActiveProject(projectId) {
    const db = await openDatabase();
    const tx = db.transaction(appStoreName, 'readwrite');
    const store = tx.objectStore(appStoreName);

    store.put({ key: lastActiveProjectIdKey, value: projectId });

    await transactionToPromise(tx);
}

function openDatabase() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(startupDbName, startupDbVersion);

        request.onupgradeneeded = (event) => {
            const db = event.target.result;

            if (!db.objectStoreNames.contains(projectStoreName)) {
                db.createObjectStore(projectStoreName, { keyPath: 'id' });
            }

            if (!db.objectStoreNames.contains(appStoreName)) {
                db.createObjectStore(appStoreName, { keyPath: 'key' });
            }
        };

        request.onsuccess = () => resolve(request.result);
        request.onerror = () => reject(request.error ?? new Error('Failed to open startup database.'));
    });
}

function requestToPromise(request) {
    return new Promise((resolve, reject) => {
        request.onsuccess = () => resolve(request.result);
        request.onerror = () => reject(request.error ?? new Error('IndexedDB request failed.'));
    });
}

function transactionToPromise(transaction) {
    return new Promise((resolve, reject) => {
        transaction.oncomplete = () => resolve();
        transaction.onabort = () => reject(transaction.error ?? new Error('IndexedDB transaction aborted.'));
        transaction.onerror = () => reject(transaction.error ?? new Error('IndexedDB transaction failed.'));
    });
}

function isProjectDocument(value) {
    return value && typeof value === 'object'
        && typeof value.id === 'string' && value.id.length > 0
        && typeof value.appVersion === 'string'
        && value.project && typeof value.project === 'object'
        && typeof value.project.id === 'string' && value.project.id.length > 0
        && Array.isArray(value.nodes);
}

function normalizeProjectDocument(projectDocument) {
    if (!projectDocument || typeof projectDocument !== 'object') {
        throw new Error('Project document is required.');
    }

    const normalized = { ...projectDocument };
    const projectId = normalized?.project?.id;

    if ((!normalized.id || normalized.id.length === 0) && typeof projectId === 'string' && projectId.length > 0) {
        normalized.id = projectId;
    }

    if (!normalized.id || normalized.id.length === 0) {
        throw new Error('Project document id is required.');
    }

    return normalized;
}
