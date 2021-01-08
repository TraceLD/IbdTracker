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

export function isInTheFuture(date: Date): boolean {
    const dateNow: Date = new Date();
    
    return date > dateNow;
}

export function isInThePast(date: Date): boolean {
    const dateNow: Date = new Date();

    return date < dateNow;
}