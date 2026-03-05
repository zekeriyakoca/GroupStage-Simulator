import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SimulationResult } from '../models/simulation.model';

@Injectable({ providedIn: 'root' })
export class SimulationService {
  constructor(private http: HttpClient) {}

  simulate(teams: { name: string; strength: number }[]): Observable<SimulationResult> {
    return this.http.post<SimulationResult>('/groups/simulate', { teams });
  }
}
