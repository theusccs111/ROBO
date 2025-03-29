import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import Menus from "../../data/menu.json";
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzIconModule } from 'ng-zorro-antd/icon';
import Menu, { MenuItem } from '../../models/menu';
import { TokenStorageService } from '../../services/token.storage.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    NzIconModule
  ]
})
export class SidebarComponent implements OnInit, OnDestroy {
  isGestor: boolean = false;
  isSidebarOpen = false;
  subMenuStates: { [key: string]: boolean } = {};
  menus: Menu[] = Menus as Menu[];
  flattedMenus: MenuItem[] = [];
  foundMenus: Observable<Menu[]> = new Observable<Menu[]>();
  behaviorSubject: BehaviorSubject<string> = new BehaviorSubject<string>("");

  constructor(private router: Router, private tokenStorageService: TokenStorageService) { }

  ngOnInit(): void {
    for (let menu of this.menus)
      this.flatMenus(menu.items);

    this.foundMenus = this
      .behaviorSubject
      .pipe(
        switchMap(
          (search: string) => {
            let found: Menu[] | undefined = [];

            if (search) {
              let foundItems = this.flattedMenus.filter(t => t.title.toLocaleLowerCase().includes(search.toLowerCase()));

              found.push({
                aggregator: 'Menus encontrados',
                items: foundItems
              });
            }
            else {
              found = this.menus;
            }

            return new Observable<Menu[]>(obs => obs.next(found));
          })
      );
  }

  ngOnDestroy(): void {
    this.behaviorSubject.unsubscribe();
  }

  flatMenus(menus: MenuItem[]): void {
    for (let menu of menus) {
      if (!menu.path)
        menu.path = '#';

      this.flattedMenus.push(menu);

      if ((menu.items?.length ?? 0) > 0)
        this.flatMenus(menu.items ?? []);
    }
  }

  /*onSearchMenuChange(): void {
    this.behaviorSubject.next(this.searchText);
  }*/

  shouldDisplaySidebar() {
    return !this.router.url.includes('login');
  }

  openSidebar(): void {
    this.isSidebarOpen = true;
  }

  closeSidebar(): void {
    this.isSidebarOpen = false;
    this.resetSubMenus();
  }

  toggleSubMenu(item: string): void {
    this.subMenuStates[item] = !this.subMenuStates[item];
  }

  isSubMenuOpen(item: string): boolean {
    return !!this.subMenuStates[item];
  }

  private resetSubMenus(): void {
    for (const key in this.subMenuStates) {
      if (this.subMenuStates.hasOwnProperty(key)) {
        this.subMenuStates[key] = false;
      }
    }
  }
}
