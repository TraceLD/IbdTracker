/**
 * TypeScript type declaration to fix false compilation error
 * when using Routify's window.
 */
declare global {
    interface Window {
        routify: any;
    }
}

export {}