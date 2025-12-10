import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  customerName = '';
  amount = 0;

  message = '';
  errorMessage = '';
  isLoading = false;

  orders: any[] = [];

  private readonly apiBaseUrl = 'http://localhost:5121/api';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.message = '';

    this.http.get<any[]>(`${this.apiBaseUrl}/orders`)
      .subscribe({
        next: (data) => {
          this.orders = data;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'No se pudieron cargar los pedidos.';
          this.isLoading = false;
        }
      });
  }

  createOrder(): void {
    this.errorMessage = '';
    this.message = '';

    if (!this.customerName || this.amount <= 0) {
      this.errorMessage = 'Completa el nombre del cliente y un monto válido.';
      return;
    }

    const body = {
      customerName: this.customerName,
      amount: this.amount
    };

    this.isLoading = true;

    this.http.post<any>(`${this.apiBaseUrl}/orders`, body)
      .subscribe({
        next: () => {
          this.message = 'Pedido registrado correctamente.';
          this.customerName = '';
          this.amount = 0;
          this.loadOrders();
        },
        error: () => {
          this.errorMessage = 'No se pudo registrar el pedido.';
          this.isLoading = false;
        }
      });
  }
}
