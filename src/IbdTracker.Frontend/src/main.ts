import HMR from '@roxi/routify/hmr'
import App from './App.svelte';

// Svelte boilerplate;
const app = HMR(App, { target: document.body }, 'routify-app');

export default app;