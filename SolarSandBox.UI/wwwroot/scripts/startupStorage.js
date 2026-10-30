const startupDbName = 'solar-sandbox-startup';
const startupDbVersion = 1;
const projectStoreName = 'projects';
const appStoreName = 'app';
const lastActiveProjectIdKey = 'lastActiveProjectId';

window.solarSandboxStartupStorage = {
    initialize,
    getLastActiveProject,
    createOrUpdateProject,
    setLastActiveProject
};

async function initialize() {
    await openDatabase();
}

async function getLastActiveProject() {
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
    const project = await requestToPromise(projectStore.get(appEntry.value));
    await transactionToPromise(projectTx);

    return project ?? null;
}

async function createOrUpdateProject(project) {
    const db = await openDatabase();
    const tx = db.transaction(projectStoreName, 'readwrite');
    const store = tx.objectStore(projectStoreName);

    store.put(project);

    await transactionToPromise(tx);
    return project;
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
