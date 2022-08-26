export interface QueryParams {
  pageNumber?: number;
  pageSize?: number;
  filters?: string;
  sortOrder?: string;
}

export interface RecipeDto {
  id: string;
  title: string;
  directions: string;
  recipeSourceLink: string;
  visibility: string;
  description: string;
  rating?: number;
}

export interface RecipeForManipulationDto {
  id: string;
  title: string;
  directions: string;
  recipeSourceLink: string;
  visibility: string;
  description: string;
  rating?: number;
}

export interface RecipeForCreationDto extends RecipeForManipulationDto { }
export interface RecipeForUpdateDto extends RecipeForManipulationDto { }

// need a string enum list?
// const StatusList = ['Status1', 'Status2', null] as const;
// export type Status = typeof StatusList[number];
// Then use as --> status: Status;
