import type { BowelMovementEventDto, BowelMovementEventsGroupedDto, PainEventAvgsDto } from "../models/dtos";
import { toHtmlInputFormat } from "./datetime";

/**
 * Represents data needed for the pain plots.
 * 
 * @interface
 */
export interface PainEventsPlotsTraces {
    countPainPlotTraces: any,
    durationPlotTraces: any,
}

/**
 * Transforms BMs data into the form plotly plots library expects.
 * 
 * @param  {Array<BowelMovementEventsGroupedDto>} data - The BMs data.
 * @returns {Array<any>} The plot traces.
 */
export function getBmsPlotTraces(data: Array<BowelMovementEventsGroupedDto>): Array<any> {
    const x: Array<string> = data.map((v) => toHtmlInputFormat(new Date(v.date + "Z")));
    let traces = [
        {
            x: x,
            y: data.map(() => 0),
            name: "Normal",
            type: "bar",
            marker: {
                color: "#22C55E",
            },
        },
        {
            x: x,
            y: data.map(() => 0),
            name: "Blood & Mucus",
            type: "bar",
            marker: {
                color: "#F97316",
            },
        },
        {
            x: x,
            y: data.map(() => 0),
            name: "Blood",
            type: "bar",
            marker: {
                color: "#EF4444",
            },
        },
        {
            x: x,
            y: data.map(() => 0),
            name: "Mucus",
            type: "bar",
            marker: {
                color: "#6366F1",
            },
        },
    ];

    for (let i: number = 0; i < data.length; i++) {
        let bmsOnDay: Array<BowelMovementEventDto> = data[i].bowelMovementEventsOnDay;

        bmsOnDay.forEach((bm) => {
            if (!bm.containedBlood && !bm.containedMucus) {
                traces[0].y[i] += 1;
            } else if (bm.containedBlood && bm.containedMucus) {
                traces[1].y[i] += 1;
            } else if (bm.containedBlood) {
                traces[2].y[i] += 1;
            } else if (bm.containedMucus) {
                traces[3].y[i] += 1;
            }
        });
    }

    return traces;
}

/**
 * Transforms pain events data into the form plotly plots library expects.
 * 
 * @param  {Array<PainEventAvgsDto>} data - The pain events data.
 * @returns {PainEventsPlotsTraces} The plots traces.
 */
export function getPainEventsPlotsTraces(data: Array<PainEventAvgsDto>): PainEventsPlotsTraces {
    const x: Array<Date> = data.map((v) => new Date(v.dateTime + "Z"));      
    const durationPlotTraces = [
        {
            x: x,
            y: data.map((v) => v.averageDuration),
            name: "Average duration (minutes)",
            type: "scatter",
            marker: {
                color: "#FACC15",
            },
        },
    ];
    const countPainPlotTraces = [
        {
            x: x,
            y: data.map((v) => v.count),
            name: "Count",
            type: "bar",
            marker: {
                color: "#6366F1",
            },
        },
        {
            x: x,
            y: data.map((v) => v.averageIntensity),
            name: "Average pain intensity (0-10)",
            type: "bar",
            marker: {
                color: "#EF4444",
            },
        },
    ];
    
    return {
        countPainPlotTraces: countPainPlotTraces,
        durationPlotTraces: durationPlotTraces
    };
}
