// import { StorangeEnum } from './../enums/storange.enum';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ApiResponsePaginatedListWithData, ApiResponseWithData } from '@app/_models/api-response-with-data';
import { environment } from '@environments/environment';
import { Observable, map } from 'rxjs';

export enum Ambiente {
  Desenvolvimento = 0,
  Homologacao = 1,
  Producao = 2,
  DevLocal = 3,
  DevLocalIAM = 4
}


export enum StorangeEnum {
    session,
    refreshTokenIam,
    tokenIam,
    c0
}
export interface IExportService<T> {
  obterRegistrosExportarExcel(filtro: any, includes: string): Promise<T[]>;
}

export interface IHistoricoAnimalService {
  obterHistoricoAnimal(idEntrada: number, termo: string, pagina: number, registrosPorPagina: number, paramExtra?: string): Promise<any>;
}

export abstract class BaseService<T> {

  // public ambiente = '';
  // private restEndpoint = '';
  // protected iamEndpointToken = '';
  // protected iamEndpointTokenInfo = '';
  // protected iamEndpointUserInfo = '';
  // protected iamEndpointRefreshToken = '';
  // protected clientIdBk = '';
  // protected clientId = '';
  // protected clientSecret = '';

  protected get UrlService() {
    return this.baseEndpoint;
  }

  constructor(public httpClient: HttpClient, private baseEndpoint?: string) {
    baseEndpoint = baseEndpoint || this.controllerName();
  }

  protected ObterHeaderJson(): HttpHeaders {
    const data = (<any>JSON.parse( localStorage.getItem(StorangeEnum.session.toString()) || '')).data;
    let headers: any  = {'Content-Type': 'application/json'}
      headers = {
        ...headers,
        'Authorization': 'Bearer ' + data?.token,
        // 'x-r-t':data.refreshToken,

      }
    return new HttpHeaders(headers);
  }
  get Token(): string{
    let token = (localStorage.getItem(StorangeEnum.tokenIam.toString()));
    if(token === 'null')
      token = null;
    return <string>token;
  }

  getEndPoint(route?: string){
    return `${environment.apiUrl}/${route || this.baseEndpoint}`;
  }

  public get(id?: any, route?: string) {
  return this.httpClient.get(`${this.getEndPoint(route)}/${id}`, { headers: this.ObterHeaderJson() });
  }

  public listPagination(pageNumber: number , pageSize: number, searchText: string, columnOrder: string, asc: boolean, route?: string): Observable<ApiResponsePaginatedListWithData<T>> {
    return <Observable<ApiResponsePaginatedListWithData<T>>>this.httpClient.get(`${this.getEndPoint(route)}?pageNumber=${pageNumber}&pageSize=${pageSize}&searchText=${searchText}&columnOrder=${columnOrder}&asc=${asc}`, { headers: this.ObterHeaderJson() });
  }

  public list(route?: string): Observable<ApiResponseWithData<T>> {
    return <Observable<ApiResponseWithData<T>>>this.httpClient.get(`${this.getEndPoint(route)}`, { headers: this.ObterHeaderJson() });
  }  

  public post(body: any, route?: string) {
    return this.httpClient.post(this.getEndPoint(route), body, { headers: this.ObterHeaderJson() });
  }

  public put(body: T, route?: string) {
    return this.httpClient.put(this.getEndPoint(route), body, { headers: this.ObterHeaderJson() });
  }

  public delete(id: any, route?: string) {
    return this.httpClient.delete(`${this.getEndPoint(route)}/${id}`, { headers: this.ObterHeaderJson() });
  }

  public cancel(id: any, route?: string) {
    return this.httpClient.patch(`${this.getEndPoint(route)}/cancel/${id}`, null, { headers: this.ObterHeaderJson()});
  }
  
  public postById(id: number, route?: string) {
    return this.httpClient.post(`${this.getEndPoint(route)}$?id=${id}`, id, { headers: this.ObterHeaderJson() });
  }

  public controllerName(){
    let classe = this.constructor.name;
    classe = classe.substring(0, classe.length - 7);
    return classe;
  }
}
