import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class FormattingService {

    constructor() { }

    public formatter(value: number | null | ''): string {
        if (value === null || value === undefined || value === '') {
            return '';
        }

        if (typeof value === 'number') {
            return value.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        }

        return value; 
    }

    public parser(value: string): number {
        if (!value) return NaN;

        const cleanedValue = value.replace(/\./g, '').replace(',', '.');
        return parseFloat(cleanedValue);
    }

}
