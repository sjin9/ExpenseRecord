import { Component,Input, OnInit } from '@angular/core';
import { Item } from '../item';
import { RecordService } from '../service/record.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-record-detail',
  templateUrl: './record-detail.component.html',
  styleUrls: ['./record-detail.component.css']
})
export class RecordDetailComponent implements OnInit{
  @Input() item?:Item;

  constructor(
    private route: ActivatedRoute,
    private recordservice:RecordService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.getItem();
  }
  
  // getItem(): void {
  //   const id = Number(this.route.snapshot.paramMap.get('id'));
  //   this.recordservice.getItem(id).subscribe(item => this.item = item);
  // }
  getItem(): void {
    const id = String(this.route.snapshot.paramMap.get('id'));
    this.recordservice.getItem(id).subscribe(item => this.item = item);
  }

  goBack(): void{
    this.location.back();
  }

  save(): void{
    if (this.item) {
      this.recordservice.updateItem(this.item).subscribe(()=>this.goBack())
    }
  }

  // ngOnDestroy(): void {
  //   if(this.getItem){this.getItem.unsubscribe()};
  //   if(this.back){this.back.unsubscribe()};
  // }

}