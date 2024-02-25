
export function getTimezoneOptions() {
    const options = Intl.DateTimeFormat().resolvedOptions();
    options.offset = new Date().getTimezoneOffset();
    return options;
}