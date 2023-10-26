import { Component,OnInit } from '@angular/core';
import { Item } from '../item';
import { RecordService } from '../service/record.service';
import { BehaviorSubject, Subject, Subscription } from 'rxjs';
// import { itemList } from '../itemlist';

@Component({
  selector: 'app-record',
  templateUrl: './record.component.html',
  styleUrls: ['./record.component.css']
})
export class RecordComponent implements OnInit {
  hideDone: boolean = false;
  sortByDescDir?: SortDir;
  sortByDateDir?: SortDir = SortDir.Asc;
  readonly SortDir = SortDir;
  // private readonly updateList$$: Subject<Item[]> = new Subject<Item[]>();
  private readonly updateList$$: Subject<void> = new Subject<void>();

  items: Item[]=[]; //inherit interface Items
  sorted_items: Item[] = [];
  sorted_byDate: Item[] = []
  // done_items: Item[] = []
  // selectedItem?: Item;

  // onSelect(item:Item):void {
  //   this.selectedItem=item;
  // }

  constructor(private recordservice:RecordService) { //initalize service

  }

  ngOnInit(): void{
    this.getRecord();

    this.sorted_byDate = this.items.sort((a, b) => {
      if (a.createdTime > b.createdTime) {
        return -1;
      } 
      else if (a.createdTime < b.createdTime) return 1;
      else return 0;
    });
    
  }

  getRecord(): void{
    this.recordservice.getRecord().subscribe(items => this.items=items) 
    //items refer to items in service, this.item refers to items in this component.ts
  }

  add(description: string): void{
    description = description.trim(); //去掉字符串里面的空格
    if (!description) { return; }
    this.recordservice.addItem({ description } as Item)
      .subscribe(item => {
        this.items.push(item);
      });
  }
  
  delete(item: Item): void {
    this.items = this.items.filter(i => i !== item);
    this.recordservice.deleteItem(item.id).subscribe();
  }

  // sortByDescDir(): void{
  //   this.sorted_items = this.items.sort((a, b) => {
  //     if (a.des < b.des) {
  //       return -1;
  //     } 
  //     else if (a.des > b.des) return 1;
  //     else return 0;
  //   });
  // }




  toggleSortByDesc(): void {
    // this.sorted_items = this.items.sort((a, b) => {
    //   if (a.des < b.des) {
    //     return -1;
    //   } 
    //   else if (a.des > b.des) return 1;
    //   else return 0;
    // });
    switch (this.sortByDescDir) {
      case SortDir.Asc:
        this.sorted_items = this.items.sort((a, b) => {
              if (a.description > b.description) {
                return -1;
              } 
              else if (a.description < b.description) return 1;
              else return 0;
            });
        this.sortByDescDir = SortDir.Desc;
        break;
      // case SortDir.Desc:
      //   this.sortByDescDir = undefined;
      //   break;
      default:
        this.sorted_items = this.items.sort((a, b) => {
          if (a.description < b.description) {
            return -1;
          } 
          else if (a.description > b.description) return 1;
          else return 0;
        });
        this.sortByDescDir = SortDir.Asc;
    }
    this.updateList$$.next();

  }

  toggleSortByDate(): void {
    switch (this.sortByDateDir) {
      case SortDir.Asc:
        this.sorted_byDate = this.items.sort((a, b) => {
          if (a.createdTime > b.createdTime) {
            return -1;
          } 
          else if (a.createdTime < b.createdTime) return 1;
          else return 0;
        });
        this.sortByDateDir = SortDir.Desc;
        break;
      // case SortDir.Desc:
      //   this.sortByDateDir = undefined;
      //   break;
      default:
        this.sorted_byDate = this.items.sort((a, b) => {
          if (a.createdTime < b.createdTime) {
            return -1;
          } 
          else if (a.createdTime > b.createdTime) return 1;
          else return 0;
        });
        this.sortByDateDir = SortDir.Asc;
    }
    this.updateList$$.next();
  }

}

enum SortDir {
  Asc = 1,
  Desc = 2
}