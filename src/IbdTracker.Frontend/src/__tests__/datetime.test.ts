import { isInTheFuture, isInThePast, toHtmlInputFormat } from "../services/datetime";

const dateInThePast: Date = new Date(1995, 11, 17);
const dateInTheFuture: Date = new Date(2021, 11, 17);

test("isInThePast returns true for a date in the past", () => {
    expect(isInThePast(dateInThePast)).toBe(true);
});

test("isInThePast returns false for current date", () => {
    expect(isInThePast(new Date())).toBe(false);
});

test("isInThePast returns false for a date in the future", () => {   
    expect(isInThePast(dateInTheFuture)).toBe(false);
});

test("isInTheFuture returns false for a date in the past", () => {
    expect(isInTheFuture(dateInThePast)).toBe(false);
});

test("isInTheFuture returns false for current date", () => {
    expect(isInTheFuture(new Date())).toBe(false);
});

test("isInTheFuture returns true for a date in the future", () => {
    expect(isInTheFuture(dateInTheFuture)).toBe(true);
});

test("toHtmlInputString returns correct output", () => {
    const input: Date = new Date("2020-05-01");
    const expected: string = "2020-05-01";
    
    expect(toHtmlInputFormat(input)).toBe(expected);
});