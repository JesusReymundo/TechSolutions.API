// src/app/pages/pricing/pricing.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

export enum PriceStrategyType {
  Standard = 'Standard',
  Discount = 'Discount',
  Dynamic = 'Dynamic'
}

@Component({
  selector: 'app-pricing',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pricing.component.html',
  styleUrls: ['./pricing.component.css']
})
export class PricingComponent implements OnInit {

  // enum disponible en el HTML
  PriceStrategyType = PriceStrategyType;

  // config actual de precios
  config: any = null;
  // lista de productos
  products: any[] = [];

  // formulario de configuración
  selectedStrategy: PriceStrategyType = PriceStrategyType.Standard;
  defaultDiscount = 10;
  defaultDemand = 1.0;

  // para resaltar un producto si lo necesitas
  selectedProductId: number | null = null;

  message = '';
  errorMessage = '';
  isLoading = false;

  private readonly apiBaseUrl = 'http://localhost:5121/api';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadConfig();
    this.loadProducts();
  }

  loadConfig(): void {
    this.http.get<any>(`${this.apiBaseUrl}/pricing/config`)
      .subscribe({
        next: (cfg) => {
          this.config = cfg;

          // Si el backend devuelve valores, los reflejamos en el formulario
          if (cfg?.defaultStrategy) {
            this.selectedStrategy = cfg.defaultStrategy as PriceStrategyType;
          }
          if (typeof cfg?.defaultDiscountPercentage === 'number') {
            this.defaultDiscount = cfg.defaultDiscountPercentage;
          }
          if (typeof cfg?.defaultDemandFactor === 'number') {
            this.defaultDemand = cfg.defaultDemandFactor;
          }
        },
        error: () => {
          console.warn('No se pudo cargar la configuración de precios.');
        }
      });
  }

  loadProducts(): void {
    this.http.get<any[]>(`${this.apiBaseUrl}/catalog/simple`)
      .subscribe({
        next: (data) => {
          this.products = data;
        },
        error: () => {
          console.warn('No se pudo cargar productos para precios.');
        }
      });
  }

  // 🔹 Método real que actualiza la config (lo usa saveConfig)
  updateConfig(): void {
    if (!this.config) this.config = {};

    const body = {
      defaultStrategy: this.selectedStrategy,
      defaultDiscountPercentage: this.defaultDiscount,
      defaultDemandFactor: this.defaultDemand
    };

    this.isLoading = true;
    this.errorMessage = '';
    this.message = '';

    this.http.put<any>(`${this.apiBaseUrl}/pricing/config`, body)
      .subscribe({
        next: (cfg) => {
          this.config = cfg;
          this.message = 'Configuración de precios actualizada correctamente.';
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'No se pudo actualizar la configuración de precios.';
          this.isLoading = false;
        }
      });
  }

  // 🔹 Alias que llama a updateConfig() (para el (ngSubmit)="saveConfig()")
  saveConfig(): void {
    this.updateConfig();
  }
}
