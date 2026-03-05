import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { API_URL } from '../tokens/api-url.token';

export const baseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const apiUrl = inject(API_URL);
  return next(req.clone({ url: `${apiUrl}${req.url}` }));
};
