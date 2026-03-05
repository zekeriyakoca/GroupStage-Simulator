export interface Team {
  id: string;
  name: string;
  strength: number;
}

export interface MatchResult {
  home: Team;
  away: Team;
  homeGoals: number;
  awayGoals: number;
  isHomeWin: boolean;
  isAwayWin: boolean;
  isDraw: boolean;
}

export interface Round {
  number: number;
  matches: MatchResult[];
}

export interface Standing {
  team: Team;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  points: number;
  goalDifference: number;
}

export interface SimulationResult {
  rounds: Round[];
  standings: Standing[];
  qualified: Team[];
}
