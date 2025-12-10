// src/app/app-routing.module.ts
import { Routes } from '@angular/router';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { PaymentsComponent } from './pages/payments/payments.component';
import { InventoryComponent } from './pages/inventory/inventory.component';
import { OrdersComponent } from './pages/orders/orders.component';
import { PricingComponent } from './pages/pricing/pricing.component';
import { CatalogComponent } from './pages/catalog/catalog.component';
import { ReportsComponent } from './pages/reports/reports.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },

  { path: 'dashboard', component: DashboardComponent, title: 'Dashboard' },
  { path: 'payments', component: PaymentsComponent, title: 'Pagos' },
  { path: 'orders', component: OrdersComponent, title: 'Órdenes' },
  { path: 'inventory', component: InventoryComponent, title: 'Inventario' },
  { path: 'pricing', component: PricingComponent, title: 'Precios' },
  { path: 'catalog', component: CatalogComponent, title: 'Catálogo' },
  { path: 'reports', component: ReportsComponent, title: 'Reportes' },

  { path: '**', redirectTo: 'dashboard' }
];
