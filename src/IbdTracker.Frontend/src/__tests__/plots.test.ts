import type { BowelMovementEventsGroupedDto, PainEventAvgsDto } from "../models/dtos";
import { getBmsPlotTraces, getPainEventsPlotsTraces } from "../services/plots";

test("bms empty array input", () => {
    const input: BowelMovementEventsGroupedDto[] = [];
    const expected = [
        {
            x: [],
            y: [],
            name: "Normal",
            type: "bar",
            marker: {
                color: "#22C55E",
            },
        },
        {
            x: [],
            y: [],
            name: "Blood & Mucus",
            type: "bar",
            marker: {
                color: "#F97316",
            },
        },
        {
            x: [],
            y: [],
            name: "Blood",
            type: "bar",
            marker: {
                color: "#EF4444",
            },
        },
        {
            x: [],
            y: [],
            name: "Mucus",
            type: "bar",
            marker: {
                color: "#6366F1",
            },
        },
    ];

    expect(getBmsPlotTraces(input)).toEqual(expected);
});

test("pain events empty array input", () => {
    const input: PainEventAvgsDto[] = [];
});