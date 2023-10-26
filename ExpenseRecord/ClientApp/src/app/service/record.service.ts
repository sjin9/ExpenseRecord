import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Observable, of} from 'rxjs';
import { Item } from '../item';
// import { itemList } from '../itemlist';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecordService {

  //recordsURL='api/itemlist'  //get request to json placeholder/web api

  // recordsURL='https://localhost:7289/api/v1/ToDoItems'
  recordsURL='https://localhost:7081/api/v1/expenserecord'
  // log: any;

  constructor(private http:HttpClient) { } //inject http into constructor

  getRecord(): Observable<Item[]> {
    // const items = of(itemList)
    // return items;
    return this.http.get<Item[]>(this.recordsURL) //return an array observable
    // .pipe(
    //   catchError(this.handleError<Item[]>('getRecord',[])) 
      //use pipe to extend observable to catch error
      //catch error will block failed observable)
  }

  getItem(id: string): Observable<Item> {
    // const item = itemList.find(i => i.des === des)!;
    // return of(item);
    const url = `${this.recordsURL}/${id}`  //change ${id} to the actual url/id
    return this.http.get<Item>(url) //return an object observable
  }

  updateItem(item:Item):Observable<any>{  //在service中增删改查
    const url = `${this.recordsURL}/${item.id}`
    return this.http.put(url, item, this.httpOptions)
    // http.put(URL 地址,要修改的数据,选项)
  } 

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }) 
    //期待在保存时的请求中有一个特殊的头
  }

  addItem(item: Item): Observable<Item> {
    return this.http.post<Item>(this.recordsURL, item, this.httpOptions)
  }

  
  deleteItem(id: string): Observable<Item> {
    const url = `${this.recordsURL}/${id}`;
    return this.http.delete<Item>(url, this.httpOptions)
  }

  // searchItem(term: string): Observable<Item[]> {
  //   if (!term.trim()) {
  //     // if not search term, return empty hero array.
  //     return of([]);
  //   }
  //   return this.http.get<Item[]>(`${this.recordsURL}/?description =${term}`)
  // }
  searchItem(term: string): Observable<Item[]> {
    if (!term.trim()) {
      // if not search term, return empty array.
      return of([]);
    }
    // if (item.description.includes('term')){
    return this.http.get<Item[]>(`${this.recordsURL}/?description =${term}`)
    
  }



  
}
