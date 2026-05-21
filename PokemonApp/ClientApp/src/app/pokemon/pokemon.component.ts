import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

interface Pokemon {
  id: number;
  name: string;
  imageUrl: string;
  types: string[];
}

interface PokemonResponse {
  totalCount: number;
  page: number;
  pageSize: number;
  items: Pokemon[];
}

interface PokemonDetail {
  id: number;
  name: string;
  imageUrl: string;
  types: string[];
  description: string;
  height: number;
  weight: number;
  abilities: string[];
  stats: {
    hp: number;
    attack: number;
    defense: number;
    specialAttack: number;
    specialDefense: number;
    speed: number;
  };
}

@Component({
  selector: 'app-pokemon',
  templateUrl: './pokemon.component.html',
})
export class PokemonComponent implements OnInit {
  pokemons: Pokemon[] = [];
  totalCount = 0;
  currentPage = 1;
  pageSize = 20;
  searchText = '';
  selectedType = '';
  isLoading = false;
  isLoadingDetails = false;
  isSaving = false;
  
  selectedPokemon: PokemonDetail | null = null;
  isDetailOpen = false;
  
  // Create Pokemon Modal States
  isCreateOpen = false;
  newPokemon = this.getDefaultNewPokemon();

  pokemonTypes = [
    'normal', 'fire', 'water', 'electric', 'grass', 'ice', 
    'fighting', 'poison', 'ground', 'flying', 'psychic', 'bug', 
    'rock', 'ghost', 'dragon', 'dark', 'steel', 'fairy'
  ];

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['search']) {
        this.searchText = params['search'];
      }
      this.loadPokemons(false);
    });
  }

  getDefaultNewPokemon() {
    return {
      name: '',
      primaryType: 'normal',
      secondaryType: '',
      description: 'Un Pokémon misterioso creado recientemente en la Pokédex.',
      height: 1.0,
      weight: 10.0,
      ability: 'ninguna',
      hp: 50,
      attack: 50,
      defense: 50,
      speed: 50
    };
  }

  loadPokemons(append = false) {
    if (this.isLoading) return;
    this.isLoading = true;

    if (!append) {
      this.currentPage = 1;
    }

    const url = `/api/pokemon?search=${encodeURIComponent(this.searchText)}&type=${this.selectedType}&page=${this.currentPage}&pageSize=${this.pageSize}`;

    this.http.get<PokemonResponse>(url).subscribe({
      next: (response) => {
        if (append) {
          this.pokemons = [...this.pokemons, ...response.items];
        } else {
          this.pokemons = response.items;
        }
        this.totalCount = response.totalCount;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading pokemons', error);
        this.isLoading = false;
      }
    });
  }

  search() {
    this.loadPokemons(false);
  }

  clearSearch() {
    this.searchText = '';
    this.loadPokemons(false);
  }

  selectType(type: string) {
    if (this.selectedType === type) {
      this.selectedType = '';
    } else {
      this.selectedType = type;
    }
    this.loadPokemons(false);
  }

  loadMore() {
    if (this.pokemons.length >= this.totalCount) return;
    this.currentPage++;
    this.loadPokemons(true);
  }

  openDetails(pokemonId: number) {
    this.isLoadingDetails = true;
    this.isDetailOpen = true;
    this.selectedPokemon = null;

    this.http.get<PokemonDetail>(`/api/pokemon/${pokemonId}`).subscribe({
      next: (detail) => {
        this.selectedPokemon = detail;
        this.isLoadingDetails = false;
      },
      error: (error) => {
        console.error('Error loading pokemon details', error);
        this.isLoadingDetails = false;
        this.closeDetails();
      }
    });
  }

  closeDetails() {
    this.isDetailOpen = false;
    setTimeout(() => {
      this.selectedPokemon = null;
    }, 300);
  }

  // Create Pokemon CRUD logic
  openCreateModal() {
    this.newPokemon = this.getDefaultNewPokemon();
    this.isCreateOpen = true;
  }

  closeCreateModal() {
    this.isCreateOpen = false;
  }

  submitCreate() {
    if (!this.newPokemon.name.trim()) {
      alert('Por favor, introduce el nombre del Pokémon.');
      return;
    }

    this.isSaving = true;

    // Map form states to API DTO structure
    const types = [this.newPokemon.primaryType];
    if (this.newPokemon.secondaryType) {
      types.push(this.newPokemon.secondaryType);
    }

    const payload = {
      name: this.newPokemon.name.trim(),
      imageUrl: '', // Controller will use placeholder
      types: types,
      description: this.newPokemon.description.trim(),
      height: this.newPokemon.height,
      weight: this.newPokemon.weight,
      abilities: [this.newPokemon.ability.trim()],
      stats: {
        hp: this.newPokemon.hp,
        attack: this.newPokemon.attack,
        defense: this.newPokemon.defense,
        specialAttack: 50,
        specialDefense: 50,
        speed: this.newPokemon.speed
      }
    };

    this.http.post<PokemonDetail>('/api/pokemon', payload).subscribe({
      next: (created) => {
        this.isSaving = false;
        this.closeCreateModal();
        // Reload list to show newly created Pokémon
        this.loadPokemons(false);
      },
      error: (error) => {
        console.error('Error creating pokemon', error);
        this.isSaving = false;
        alert('Error al guardar el Pokémon.');
      }
    });
  }

  deletePokemon(id: number) {
    if (!confirm('¿Estás seguro de que quieres liberar este Pokémon? Se borrará de la base de datos permanentemente.')) {
      return;
    }

    this.http.delete(`/api/pokemon/${id}`).subscribe({
      next: () => {
        this.closeDetails();
        // Reload list
        this.loadPokemons(false);
      },
      error: (error) => {
        console.error('Error deleting pokemon', error);
        alert('Error al liberar el Pokémon.');
      }
    });
  }

  getStatPercentage(value: number): string {
    return `${Math.min(100, (value / 160) * 100)}%`;
  }

  getStatColor(statName: string): string {
    switch (statName) {
      case 'hp': return 'bg-green-500';
      case 'attack': return 'bg-red-500';
      case 'defense': return 'bg-blue-500';
      case 'specialAttack': return 'bg-purple-500';
      case 'specialDefense': return 'bg-teal-500';
      case 'speed': return 'bg-yellow-500';
      default: return 'bg-gray-500';
    }
  }

  getStatLabel(statName: string): string {
    switch (statName) {
      case 'hp': return 'PS';
      case 'attack': return 'Ataque';
      case 'defense': return 'Defensa';
      case 'specialAttack': return 'At. Esp';
      case 'specialDefense': return 'Def. Esp';
      case 'speed': return 'Velocidad';
      default: return statName;
    }
  }
}
