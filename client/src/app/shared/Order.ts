 

    export class  OrderITem {
        id: number;
        quantity: number;
        unitPrice: number;
        productId: number;
        productCategory: string;
        productSize: string;
        productPrice: number;
        productTitle: string;
        productArtId: string;
    }

export class Order {
    orderId: number;
    orderDate: Date = new Date();
    orderNumber: string = Math.random().toString(36).substr(2,5);
    iTems: OrderITem[] = [];
    get subtotal(): number {

        const result = this.iTems.reduce((tot, val) => {
            return tot + (val.unitPrice * val.quantity)
        },0)
        return result;
    }

    }

 
