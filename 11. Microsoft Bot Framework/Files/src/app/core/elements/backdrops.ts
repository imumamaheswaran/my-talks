import { Component } from '@angular/core';

declare var $: any;

@Component({
  selector: 'backdrops',
  templateUrl: '../elements/backdrops.html'
})

export class BackdropsComponent {

  constructor() {
  }

  leftSidebarBackdrop(): void {
    const el = $('.left-sidebar-backdrop');
    el.removeClass('fade in');
    el.removeClass('in');
    $('body').attr('data-collapsed', false);
  }

  rightSidebarBackdrop(): void {
    const el = $('.right-sidebar-backdrop');
    el.removeClass('fade');
    el.removeClass('in');
    $('.right-sidebar-outer').removeClass('show-from-right');
  }

}
