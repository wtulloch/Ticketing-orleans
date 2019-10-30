declare const SERVICE_URL: string;

// Gets set by the webpack Define plugin at build time
const _SERVICE_URL = SERVICE_URL;

export {_SERVICE_URL as SERVICE_URL};