// src/app/models/api.models.ts

// -------- PAGOS --------
export enum PaymentMethod {
  PayPal = 0,
  Yape = 1,
  Plin = 2
}

export interface PaymentRequest {
  amount: number;
  currency: string;
  method: PaymentMethod;
  customerIdentifier: string;
}

export interface PaymentResult {
  success: boolean;
  transactionId?: string;
  errorMessage?: string;
}

export interface PaymentConfig {
  enabledMethods: string[];
}

// -------- INVENTARIO --------
export interface InventoryItem {
  productId: number;
  productName: string;
  stock: number;
  minimumStock: number;
}

export interface StockNotification {
  recipientRole: string;
  message: string;
}

// -------- PEDIDOS --------
export enum OrderStatus {
  Created = 0,
  Processed = 1,
  Cancelled = 2
}

export interface Order {
  id: number;
  customerName: string;
  amount: number;
  discountPercentage: number;
  status: OrderStatus;
  createdAt: string;
  finalAmount: number;
}

// -------- PRECIOS / CATÁLOGO --------
export interface Product {
  id: number;
  name: string;
  basePrice: number;
}

export enum PriceStrategyType {
  Standard = 0,
  Discount = 1,
  Dynamic = 2
}

export interface PricingConfig {
  defaultStrategy: string;
  defaultDiscountPercentage?: number;
  defaultDemandFactor?: number;
}

export interface CatalogResponse {
  pageNumber: number;
  pageSize: number;
  totalItems: number;
  items: Product[];
}

// -------- REPORTES --------
export interface FinancialReport {
  year: number;
  month: number;
  totalSales: number;
  totalCosts: number;
  profit: number;
  generatedAt: string;
  generatedBy: string;
}
