import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SimulationService } from './services/simulation.service';
import { SimulationResult } from './models/simulation.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  teams = [
    { name: 'Brazil', strength: 85 },
    { name: 'Argentina', strength: 82 },
    { name: 'France', strength: 88 },
    { name: 'Germany', strength: 78 },
  ];

  result: SimulationResult | null = null;
  loading = false;
  error: string | null = null;

  constructor(private simulationService: SimulationService) {}

  rowClass(i: number): string {
    const base = 'border-b';
    const bg = i < 2 ? ' bg-emerald-950' : '';
    const border = i === 1 ? ' border-emerald-800 border-b-2' : ' border-slate-800';
    return base + bg + border;
  }

  positionBadgeClass(i: number): string {
    if (i === 0) return 'bg-yellow-500 text-yellow-950';
    if (i === 1) return 'bg-slate-400 text-slate-900';
    return 'bg-slate-700 text-slate-400';
  }

  gdClass(gd: number): string {
    if (gd > 0) return 'text-emerald-400';
    if (gd < 0) return 'text-red-400';
    return 'text-slate-500';
  }

  matchTeamClass(isWinner: boolean): string {
    return isWinner ? 'text-slate-100 font-semibold' : 'text-slate-500';
  }

  simulate(): void {
    this.loading = true;
    this.error = null;
    this.result = null;
    this.simulationService.simulate(this.teams).subscribe({
      next: (result) => {
        this.result = result;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to connect to the API. Make sure the backend is running on port 5000.';
        this.loading = false;
      }
    });
  }
}
