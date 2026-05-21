import { Component } from '@angular/core';

interface Starter {
  id: number;
  name: string;
  type: string;
  color: string;
  borderColor: string;
  imageUrl: string;
  description: string;
  stats: {
    hp: number;
    attack: number;
    defense: number;
    speed: number;
  };
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  isPokeballOpen = false;
  pokeballStatusText = '¡Haz clic en la Pokéball para abrirla!';
  selectedStarter: Starter | null = null;
  revealedPokemonId: number = 25; // Pikachu silhouette or default inside Pokéball

  starters: Starter[] = [
    {
      id: 1,
      name: 'Bulbasaur',
      type: 'grass',
      color: 'bg-green-600/20 hover:bg-green-600/30',
      borderColor: 'border-green-500/40 hover:border-green-500',
      imageUrl: 'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/1.png',
      description: 'Una rara semilla fue plantada en su lomo al nacer. La planta brota y crece con este Pokémon.',
      stats: { hp: 45, attack: 49, defense: 49, speed: 45 }
    },
    {
      id: 4,
      name: 'Charmander',
      type: 'fire',
      color: 'bg-red-600/20 hover:bg-red-600/30',
      borderColor: 'border-red-500/40 hover:border-red-500',
      imageUrl: 'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/4.png',
      description: 'Prefiere claramente los lugares calientes. Cuando llueve, se dice que sale vapor de la punta de su cola.',
      stats: { hp: 39, attack: 52, defense: 43, speed: 65 }
    },
    {
      id: 7,
      name: 'Squirtle',
      type: 'water',
      color: 'bg-blue-600/20 hover:bg-blue-600/30',
      borderColor: 'border-blue-500/40 hover:border-blue-500',
      imageUrl: 'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/7.png',
      description: 'Después del nacimiento, su espalda se hincha y se endurece en un caparazón. Rocía espuma potentemente.',
      stats: { hp: 44, attack: 48, defense: 65, speed: 43 }
    }
  ];

  togglePokeball() {
    this.isPokeballOpen = !this.isPokeballOpen;
    if (this.isPokeballOpen) {
      this.pokeballStatusText = '¡Has liberado un misterio! Haz clic de nuevo para cerrarla.';
      // Randomize pokemon inside Pokéball (between 1 and 151)
      this.revealedPokemonId = Math.floor(Math.random() * 151) + 1;
    } else {
      this.pokeballStatusText = '¡Haz clic en la Pokéball para abrirla!';
    }
  }

  selectStarter(starter: Starter) {
    this.selectedStarter = starter;
  }

  getStatPercentage(value: number): string {
    return `${Math.min(100, (value / 120) * 100)}%`;
  }
}
