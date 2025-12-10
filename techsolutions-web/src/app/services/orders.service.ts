import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly baseUrl = 'http://localhost:5121/api/Orders';

  constructor(private http: HttpClient) {}

  getOrders(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  createOrder(customerName: string, amount: number): Observable<any> {
    return this.http.post<any>(this.baseUrl, { customerName, amount });
  }
}
