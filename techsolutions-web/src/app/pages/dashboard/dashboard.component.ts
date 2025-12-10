import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  kpis = [
    { label: 'Productos en catálogo', value: 120 },
    { label: 'Pedidos del mes', value: 34 },
    { label: 'Pagos procesados', value: 28 },
    { label: 'Stock bajo alerta', value: 7 }
  ];
}
