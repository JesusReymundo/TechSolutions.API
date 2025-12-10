import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ReportsService {
  private readonly baseUrl = 'http://localhost:5121/api/Reports';

  constructor(private http: HttpClient) {}

  getMonthlyReport(
    year: number,
    month: number,
    userName?: string,
    role?: string,
  ): Observable<any> {
    let params = new HttpParams()
      .set('year', year)
      .set('month', month);

    if (userName) params = params.set('userName', userName);
    if (role) params = params.set('role', role);

    return this.http.get<any>(`${this.baseUrl}/monthly`, { params });
  }
}

