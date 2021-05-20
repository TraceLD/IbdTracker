/**
 * Creates an array containing a range of numbers between two numbers.
 * 
 * @param start - start number.
 * @param end - end number.
 * @returns Array containing a range of numbers between two numbers.
 */
export function range(start: number, end: number): Array<number> {
    if (start > end) {
        throw new Error("Start can not be bigger than end.");
    }

    let numbers: Array<number> = [];
    
    for (let i = start; i < end; i++) {
        numbers.push(i);
    }

    return numbers;
}