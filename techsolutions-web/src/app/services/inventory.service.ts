import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class InventoryService {
  private readonly baseUrl = 'http://localhost:5121/api/Inventory';

  constructor(private http: HttpClient) {}

  getInventory(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  adjustStock(productId: number, delta: number): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/adjust`, { productId, delta });
  }

  updateMinimumStock(productId: number, minimumStock: number): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/minimumStock`, { productId, minimumStock });
  }
}
