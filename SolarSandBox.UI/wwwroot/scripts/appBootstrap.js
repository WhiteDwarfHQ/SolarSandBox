(() => {
    const app = {
        registerServiceWorker,
        getPrefersDarkMode,
        getThemePreference,
        setThemePreference,
        subscribeToThemePreference,
        unsubscribeFromThemePreference,
        downloadTextFile
    };

    let themeMediaQuery = null;
    let themeHandler = null;

    window.solarSandboxApp = app;

    initializeDelegatedButtonHandling();
    registerServiceWorker();

    function registerServiceWorker() {
        if (!('serviceWorker' in navigator)) {
            return;
        }

        if (!shouldRegisterServiceWorker()) {
            return;
        }

        navigator.serviceWorker.register('service-worker.js');
    }

    function shouldRegisterServiceWorker() {
        const host = window.location.hostname;
        return host !== 'localhost' && host !== '127.0.0.1' && host !== '::1';
    }

    function getPrefersDarkMode() {
        if (!window.matchMedia) {
            return false;
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches;
    }

    function getThemePreference() {
        try {
            return localStorage.getItem('solarSandbox.themeMode');
        } catch {
            return null;
        }
    }

    function setThemePreference(themeMode) {
        try {
            if (!themeMode) {
                localStorage.removeItem('solarSandbox.themeMode');
                return;
            }

            localStorage.setItem('solarSandbox.themeMode', themeMode);
        } catch {
            // ignore storage failures
        }
    }

    function subscribeToThemePreference(dotNetRef) {
        if (!window.matchMedia) {
            return;
        }

        unsubscribeFromThemePreference();

        themeMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
        themeHandler = async (event) => {
            await dotNetRef.invokeMethodAsync('OnSystemThemeChanged', event.matches);
        };

        if (themeMediaQuery.addEventListener) {
            themeMediaQuery.addEventListener('change', themeHandler);
        } else if (themeMediaQuery.addListener) {
            themeMediaQuery.addListener(themeHandler);
        }
    }

    function unsubscribeFromThemePreference() {
        if (!themeMediaQuery || !themeHandler) {
            return;
        }

        if (themeMediaQuery.removeEventListener) {
            themeMediaQuery.removeEventListener('change', themeHandler);
        } else if (themeMediaQuery.removeListener) {
            themeMediaQuery.removeListener(themeHandler);
        }

        themeMediaQuery = null;
        themeHandler = null;
    }

    function downloadTextFile(fileName, content, mimeType) {
        const blob = new Blob([content], { type: mimeType || 'application/json;charset=utf-8' });
        const url = URL.createObjectURL(blob);

        const anchor = document.createElement('a');
        anchor.href = url;
        anchor.download = fileName;
        anchor.style.display = 'none';

        document.body.appendChild(anchor);
        anchor.click();

        document.body.removeChild(anchor);
        URL.revokeObjectURL(url);
    }

    function initializeDelegatedButtonHandling() {
        document.addEventListener('click', (event) => {
            const button = event.target.closest('button[data-js-action]');
            if (!button) {
                return;
            }

            const action = button.dataset.jsAction;
            if (!action) {
                return;
            }

            window.dispatchEvent(new CustomEvent('solarsandbox:button-action', {
                detail: {
                    action,
                    elementId: button.id || null
                }
            }));
        });
    }
})();
