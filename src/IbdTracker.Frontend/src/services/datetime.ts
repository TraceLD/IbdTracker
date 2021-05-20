/**
 * Combines an HTML date and time inputs into a @see Date .
 * 
 * @param dateInput Date HTML input
 * @param timeInput Time HTML input
 * @returns Date and time HTML inputs combined into a @see Date .
 */
export function combineInputs(dateInput: string, timeInput: string): Date {
    let date: Date = new Date(dateInput);
    let arr: Array<string> = timeInput.split(":");
    let intArr: Array<number> = arr.map((el: string) => {
        return parseInt(el);
    });
    
    date.setHours(intArr[0]);
    date.setMinutes(intArr[1]);

    return date;
}

/**
 * Checks whether a date is in the future.
 * 
 * @param date - Date to check.
 * @returns Whether the date is in the future.
 */
export function isInTheFuture(date: Date): boolean {
    const dateNow: Date = new Date();
    
    return date > dateNow;
}

/**
 * Checks whether a date is in the past.
 * 
 * @param date - Date to check.
 * @returns Whether the date is in the past.
 */
export function isInThePast(date: Date): boolean {
    const dateNow: Date = new Date();

    return date < dateNow;
}

/**
 * Converts a date into the HTML input format string.
 * 
 * The HTML input string is in the "YYYY-MM-DD" format.
 * 
 * @param date - Date to convert.
 * @returns Date in the HTML input format.
 */
export function toHtmlInputFormat(date: Date): string {
    return date.toISOString().slice(0,10);
}