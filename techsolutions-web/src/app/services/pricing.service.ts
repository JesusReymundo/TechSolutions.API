import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum PriceStrategyType {
  Standard = 0,
  Discount = 1,
  Dynamic = 2,
}

@Injectable({ providedIn: 'root' })
export class PricingService {
  private readonly baseUrl = 'http://localhost:5121/api/Pricing';

  constructor(private http: HttpClient) {}

  getConfig(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/config`);
  }

  updateConfig(
    defaultStrategy: PriceStrategyType,
    defaultDiscount: number,
    defaultDemand: number,
  ): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/config`, {
      defaultStrategy,
      defaultDiscountPercentage: defaultDiscount,
      defaultDemandFactor: defaultDemand,
    });
  }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/products`);
  }
}
