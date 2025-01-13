import {
  ApplicationConfig,
  provideExperimentalZonelessChangeDetection,
} from "@angular/core";
import { provideRouter } from "@angular/router";

import { APP_ROUTES } from "./app.routes";
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from "@angular/common/http";
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { errorInterceptor } from "@core/interceptors/error-interceptor.interceptor";
import { ApiModule, Configuration } from "@myschedulerapp/api-client";
import { ApiConfigService } from "@core/config/api.config";

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES),
    provideHttpClient(withFetch(), withInterceptors([errorInterceptor])),
    provideExperimentalZonelessChangeDetection(),
    provideAnimationsAsync(),
    ApiModule,
    {
      provide: Configuration,
      useFactory: (apiConfigService: ApiConfigService) =>
        apiConfigService.createConfiguration(),
      deps: [ApiConfigService],
    },
  ],
};
