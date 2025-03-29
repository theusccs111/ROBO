import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class DataResetService {

    resetData: EventEmitter<any> = new EventEmitter<any>();

    constructor() { }
}
