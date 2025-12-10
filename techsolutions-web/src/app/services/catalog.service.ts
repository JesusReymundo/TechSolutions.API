import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly baseUrl = 'http://localhost:5121/api/Catalog';

  constructor(private http: HttpClient) {}

  getCatalog(pageNumber: number, pageSize: number, nameFilter: string): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize)
      .set('nameFilter', nameFilter ?? '');

    return this.http.get<any>(this.baseUrl, { params });
  }
}
