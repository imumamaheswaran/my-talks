import { Component } from '@angular/core';

declare var $: any;

@Component({
  selector: 'right-sidebar-1',
  templateUrl: '../elements/right-sidebar-1.html'
})

export class RightSidebar1Component {

  constructor() {
  }

  setBackground(value): void {
    $('body').attr('data-background', value);
    $('body').trigger('changed:background');
  }
  setNavbar(value): void {
    $('body').attr('data-navbar', value);
    $('body').trigger('changed:navbar');
  }
  setLeftSidebar(value): void {
    $('body').attr('data-sidebar', value);
    $('body').trigger('changed:sidebar');
  }
  setTopNavigation(value): void {
    $('body').attr('data-top-navigation', value);
    $('body').trigger('changed:top-navigation');
  }
  setCollapsed(value): void {
    $('body').attr('data-collapsed', value);
    $('body').trigger('toggle:collapsed');
  }

}
