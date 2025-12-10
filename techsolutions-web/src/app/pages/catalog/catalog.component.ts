import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.css']
})
export class CatalogComponent implements OnInit {
  nameFilter = '';
  pageSize = 5;

  page: any | null = null;
  isLoading = false;
  errorMessage = '';

  private readonly apiBaseUrl = 'http://localhost:5121/api';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadPage(1);
  }

  loadPage(pageNumber: number = 1): void {
    if (pageNumber < 1) pageNumber = 1;

    this.isLoading = true;
    this.errorMessage = '';

    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', this.pageSize.toString())
      .set('nameFilter', this.nameFilter ?? '');

    this.http.get<any>(`${this.apiBaseUrl}/catalog`, { params })
      .subscribe({
        next: (result) => {
          this.page = result;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'No se pudo cargar el catálogo de productos.';
          this.isLoading = false;
        }
      });
  }

  applyFilters(): void {
    this.loadPage(1);
  }
}
