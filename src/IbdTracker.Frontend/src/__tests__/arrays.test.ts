import { range } from "../services/arrays";

test("range returns an empty array if range is 0", () => {
    expect(range(0 ,0)).toStrictEqual([]);
});

test("range returns an error if end bigger than start", () => {
    expect(() => range(10, 0)).toThrowError();
});

test("range should return correct range if inputs are correct", () => {
    expect(range(0, 2)).toStrictEqual([0, 1]);
});