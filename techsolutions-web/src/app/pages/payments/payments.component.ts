import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

export enum PaymentMethod {
  PayPal = 'PayPal',
  Yape = 'Yape',
  Plin = 'Plin'
}

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.css']
})
export class PaymentsComponent implements OnInit {

  PaymentMethod = PaymentMethod;

  enabledConfig = false;
  config: any = null;

  methodsList: PaymentMethod[] = [
    PaymentMethod.PayPal,
    PaymentMethod.Yape,
    PaymentMethod.Plin
  ];

  amount = 0;
  currency = 'PEN';
  customerIdentifier = '';
  method: PaymentMethod = PaymentMethod.PayPal;

  result: any = null;
  errorMessage = '';
  message = '';
  isSubmitting = false;

  private readonly apiBaseUrl = 'http://localhost:5121/api';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadConfig();
  }

  enableConfig(): void {
    this.enabledConfig = true;
    this.message = '';
    this.errorMessage = '';
  }

  disableConfig(): void {
    this.enabledConfig = false;
    this.message = 'Los pagos están deshabilitados.';
  }

  private loadConfig(): void {
    this.http.get<any>(`${this.apiBaseUrl}/payments/config`)
      .subscribe({
        next: (cfg) => {
          this.config = cfg;
        },
        error: () => {
          console.warn('No se pudo cargar la configuración de pagos.');
        }
      });
  }

  processPayment(): void {
    this.errorMessage = '';
    this.message = '';
    this.result = null;

    if (!this.enabledConfig) {
      this.errorMessage = 'Los pagos están deshabilitados.';
      return;
    }

    if (this.amount <= 0) {
      this.errorMessage = 'El monto debe ser mayor a 0.';
      return;
    }

    this.isSubmitting = true;

    const body = {
      amount: this.amount,
      currency: this.currency,
      customerIdentifier: this.customerIdentifier,
      method: this.method
    };

    this.http.post<any>(`${this.apiBaseUrl}/payments`, body)
      .subscribe({
        next: (res) => {
          this.result = res;
          this.message = 'Pago procesado correctamente.';
          this.isSubmitting = false;
        },
        error: () => {
          this.errorMessage = 'No se pudo procesar el pago.';
          this.isSubmitting = false;
        }
      });
  }
}
