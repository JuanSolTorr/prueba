import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Item {
  id: number;
  name: string;
  category: string;
  effect: string;
  cost: number;
  imageUrl: string;
}

interface ItemResponse {
  totalCount: number;
  page: number;
  pageSize: number;
  items: Item[];
  categories: string[];
}

@Component({
  selector: 'app-objects',
  templateUrl: './objects.component.html',
})
export class ObjectsComponent implements OnInit {
  items: Item[] = [];
  categories: string[] = [];
  totalCount = 0;
  currentPage = 1;
  pageSize = 20;
  searchText = '';
  selectedCategory = '';
  isLoading = false;

  selectedItem: Item | null = null;
  isDetailOpen = false;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadItems(false);
  }

  loadItems(append = false) {
    if (this.isLoading) return;
    this.isLoading = true;

    if (!append) {
      this.currentPage = 1;
    }

    const url = `/api/items?search=${encodeURIComponent(this.searchText)}&category=${this.selectedCategory}&page=${this.currentPage}&pageSize=${this.pageSize}`;

    this.http.get<ItemResponse>(url).subscribe({
      next: (response) => {
        if (append) {
          this.items = [...this.items, ...response.items];
        } else {
          this.items = response.items;
        }
        this.totalCount = response.totalCount;
        this.categories = response.categories;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading items', error);
        this.isLoading = false;
      }
    });
  }

  search() {
    this.loadItems(false);
  }

  clearSearch() {
    this.searchText = '';
    this.loadItems(false);
  }

  selectCategory(category: string) {
    if (this.selectedCategory === category) {
      this.selectedCategory = ''; // Toggle off
    } else {
      this.selectedCategory = category;
    }
    this.loadItems(false);
  }

  loadMore() {
    if (this.items.length >= this.totalCount) return;
    this.currentPage++;
    this.loadItems(true);
  }

  openDetails(item: Item) {
    this.selectedItem = item;
    this.isDetailOpen = true;
  }

  closeDetails() {
    this.isDetailOpen = false;
    setTimeout(() => {
      this.selectedItem = null;
    }, 300); // Wait for transition out
  }
}
