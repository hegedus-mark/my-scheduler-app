import { Injectable } from "@angular/core";
import { environment } from "@environments";
import {
  Configuration,
  ConfigurationParameters,
} from "@myschedulerapp/api-client";

@Injectable({
  providedIn: "root",
})
export class ApiConfigService {
  //authservice will be injected here to get token for api

  createConfiguration(): Configuration {
    const params: ConfigurationParameters = {
      basePath: environment.apiUrl,
    };
    return new Configuration(params);
  }
}
