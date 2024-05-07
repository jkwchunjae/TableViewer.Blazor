
export function getTimezoneOptions() {
    const options = Intl.DateTimeFormat().resolvedOptions();
    options.offset = new Date().getTimezoneOffset();
    return options;
}

export function getLanguage() {
    const language = navigator.language;
    return language;
}