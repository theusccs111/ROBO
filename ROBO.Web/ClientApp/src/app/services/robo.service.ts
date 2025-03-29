import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import ApiResponse from '../models/api-response';

var url = environment.api + '/Robo';

@Injectable({
    providedIn: 'root',
})
export class RoboService {
    constructor(private http: HttpClient) { }

    iniciar(): Observable<ApiResponse<any>> {
        let query = `${url}/`;
        return this.http.get<ApiResponse<any>>(query);
    }

    obter(): Observable<ApiResponse<any>> {
        let query = `${url}/obter-robo`;
        return this.http.get<ApiResponse<any>>(query);
    }

    mover(data: any): Observable<ApiResponse<any>> {
        let query = `${url}/Mover`;
        return this.http.put<ApiResponse<any>>(query, data);
    }

}