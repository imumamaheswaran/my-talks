import { Component } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

declare var _: any;

@Component({
  selector: 'jumbotron-2',
  templateUrl: '../elements/jumbotron-2.html'
})

export class Jumbotron2Component {

  public controller: string;
  public view: string;

  constructor(private router: Router) {

    this.controller = 'Dashboards';
    this.view = 'Dashboard';

    const self = this;
    router.events.subscribe((val) => {
      if (val instanceof NavigationStart) {
        const copy = Object.assign({}, val);
        const data = copy.url
          .split('/')
          .filter(url => url.length > 0);
        if (data.length == 2) {
          self.controller = _.startCase(data[0]).replace(/-/gi, ' ');
          self.view = _.startCase(data[1]).replace(/-/gi, ' ');
        }
      }
    });

  }

}
