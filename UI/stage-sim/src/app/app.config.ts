import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { API_URL } from './tokens/api-url.token';
import { baseUrlInterceptor } from './interceptors/base-url.interceptor';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([baseUrlInterceptor])),
    { provide: API_URL, useValue: environment.apiUrl },
  ],
};
