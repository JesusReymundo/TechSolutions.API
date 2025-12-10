import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {

  items: any[] = [];
  deltas: { [productId: number]: number } = {};
  newMinimums: { [productId: number]: number } = {};

  message = '';
  errorMessage = '';
  notifications: string[] = [];
  isLoading = false;

  private readonly apiBaseUrl = 'http://localhost:5121/api';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadInventory();
  }

  private loadInventory(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.message = '';

    this.http.get<any[]>(`${this.apiBaseUrl}/inventory`)
      .subscribe({
        next: (items) => {
          this.items = items;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'No se pudo cargar el inventario.';
          this.isLoading = false;
        }
      });
  }

  adjustStock(item: any): void {
    const delta = Number(this.deltas[item.productId] ?? 0);
    if (!delta) {
      this.message = 'Ingresa un ajuste distinto de 0.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.message = '';

    const body = { productId: item.productId, delta };

    this.http.post<any>(`${this.apiBaseUrl}/inventory/adjust`, body)
      .subscribe({
        next: (response) => {
          this.message = 'Stock actualizado correctamente.';
          if (response && Array.isArray(response.notifications)) {
            this.notifications = response.notifications;
          }
          this.deltas[item.productId] = 0;
          this.loadInventory();
        },
        error: () => {
          this.errorMessage = 'No se pudo actualizar el stock.';
          this.isLoading = false;
        }
      });
  }

  updateMinimumStock(item: any): void {
    const newMin = Number(this.newMinimums[item.productId] ?? 0);
    if (newMin <= 0) {
      this.message = 'El stock mínimo debe ser mayor a 0.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.message = '';

    const body = { productId: item.productId, minimumStock: newMin };

    this.http.put<any>(`${this.apiBaseUrl}/inventory/minimum-stock`, body)
      .subscribe({
        next: () => {
          this.message = 'Stock mínimo actualizado correctamente.';
          this.newMinimums[item.productId] = 0;
          this.loadInventory();
        },
        error: () => {
          this.errorMessage = 'No se pudo actualizar el stock mínimo.';
          this.isLoading = false;
        }
      });
  }
}
