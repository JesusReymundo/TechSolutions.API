import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent {

  year: number = new Date().getFullYear();
  month: number = new Date().getMonth() + 1;

  userName = '';
  role = '';

  report: any = null;
  errorMessage = '';
  isLoading = false;

  private readonly apiBaseUrl = 'http://localhost:5121/api';

  constructor(private http: HttpClient) {}

  generateReport(): void {
    this.errorMessage = '';
    this.report = null;
    this.isLoading = true;

    const params = new HttpParams()
      .set('year', this.year.toString())
      .set('month', this.month.toString())
      .set('userName', this.userName ?? '')
      .set('role', this.role ?? '');

    this.http.get<any>(`${this.apiBaseUrl}/reports/monthly`, { params })
      .subscribe({
        next: (rep) => {
          this.report = rep;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'No se pudo generar el reporte mensual.';
          this.isLoading = false;
        }
      });
  }
}
