pipeline {
    agent any 
    
    environment {
        PATH = "/usr/local/bin:/Users/samboers/google-cloud-sdk/bin:$PATH"
    }

    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/Samboers2001/ProductMicroservice'
            }
        }


        stage('Build docker image') {
            steps {
                script {
                    dir('/Users/samboers/development/order_management_system/ProductMicroservice') {
                        sh 'docker build -t samboers/productmicroservice .'
                    }
                }
            }
        }
        
        stage('Push to dockerhub') {
            steps {
                script {
                    withCredentials([string(credentialsId: 'dockerhubpasswordcorrect', variable: 'dockerhubpwd')]) {
                        sh 'docker login -u samboers -p ${dockerhubpwd}'
                        sh 'docker push samboers/productmicroservice'
                    }
                }
            }
        }

        stage('Deploy Database to Kubernetes') {
            steps {
                script {
                    dir('/Users/samboers/development/order_management_system/ProductMicroservice/K8S/product-database') {
                        sh 'kubectl apply -f mariadb-product-secret.yaml'
                        sh 'kubectl apply -f mariadb-product-claim.yaml'
                        sh 'kubectl apply -f mariadb-product-depl.yaml'
                    }
                }
            }
        }

        stage('Deploy ProductMicroservice to Kubernetes') {
            steps {
                script {
                    dir('/Users/samboers/development/order_management_system/ProductMicroservice/K8S/product-service') {
                        sh 'kubectl apply -f product-depl.yaml'
                        sh 'kubectl apply -f product-service-hpa.yaml'
                    }
                }
            }
        }

        stage('Rollout Restart') {
            steps {
                script {
                    sh 'kubectl rollout restart deployment product-depl'
                }
            }
        }


        stage('Wait for Deployment to be Ready') {
            steps {
                script {
                    dir('/Users/samboers/development/order_management_system/ProductMicroservice-service') {
                        sh 'kubectl wait --for=condition=available --timeout=60s deployment/product-depl'
                    }
                }
            }
        }

        stage('Load Testing') {
            steps {
                script {
                    sh 'rm -f /Users/samboers/JMeter/ProductLoadTestResults.csv'
                    sh 'rm -rf /Users/samboers/JMeter/ProductHtmlReport/*' 
                    sh 'mkdir -p /Users/samboers/JMeter/ProductHtmlReport'
                    sh '/opt/homebrew/bin/jmeter -n -t /Users/samboers/JMeter/GetAllProductsLoadTest.jmx -l /Users/samboers/JMeter/ProductLoadTestResults.csv -e -o /Users/samboers/JMeter/ProductHtmlReport'
                }
            }
        }

    }

}
