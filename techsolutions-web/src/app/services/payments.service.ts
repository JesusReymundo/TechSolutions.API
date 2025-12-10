import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum PaymentMethod {
  PayPal = 0,
  Yape = 1,
  Plin = 2,
}

@Injectable({ providedIn: 'root' })
export class PaymentsService {
  private readonly baseUrl = 'http://localhost:5121/api/Payments';

  constructor(private http: HttpClient) {}

  getConfig(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/config`);
  }

  enableConfig(): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/config/enable`, {});
  }

  disableConfig(): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/config/disable`, {});
  }

  processPayment(
    amount: number,
    currency: string,
    method: PaymentMethod,
    customerIdentifier: string,
  ): Observable<any> {
    return this.http.post<any>(this.baseUrl, {
      amount,
      currency,
      method,
      customerIdentifier,
    });
  }
}
