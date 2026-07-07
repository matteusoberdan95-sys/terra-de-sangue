# Gameplay e Combate

## Loop principal

Entrar em uma arena, ler inimigos, controlar espaco, quebrar defesa, finalizar, avancar, coletar memoria, enfrentar chefe.

## Controles iniciais

- `WASD`: movimento.
- `J`: ataque leve.

Controles futuros:

- Ataque pesado.
- Esquiva.
- Agarrao.
- Especial espiritual.
- Interacao.

## Regras de combate

- O eixo Y define profundidade, prioridade visual e posicionamento tatico.
- Ataques precisam ter antecipacao, impacto e recuperacao.
- Inimigos devem te cercar, mas nao atacar todos ao mesmo tempo.
- Dano deve ser muito legivel: flash, pausa curta, sangue, knockback e som.
- O dano deve acumular consequencia visual: sangue, roupa rasgada, ferida aberta, membro comprometido e morte brutal.
- O tipo de arma e o grau do golpe definem o nivel de mutilacao possivel.

## Kit inicial do protagonista

- Ataque leve rapido.
- Ataque pesado lento.
- Combo de 3 golpes.
- Agarrao em inimigo tonto.
- Execucao curta quando inimigo esta quase morto.
- Furia ancestral com duracao limitada.

## Estados minimos

Player:

- Idle
- Walk
- LightAttack
- HeavyAttack
- HitStun
- Grab
- Execute
- Dead

Enemy:

- Idle
- Approach
- Strafe
- Attack
- HitStun
- Knockdown
- Dead

Gore futuro:

- Light wound
- Heavy wound
- Clothing torn
- Bleeding
- Dismembered
- Executed

## Criterio de qualidade

O combate so avanca para conteudo novo quando o prototipo basico ja parecer bom com placeholders. Se o jogo nao for divertido com formas simples, arte final nao vai salvar.

## Direcao gore

O jogo deve aceitar sistemas de gore progressivo: sangue no chao e na mata, cortes por arma, roupas rasgadas, cabeca cortada, barriga aberta, visceras, membros decepados e corpo partido em execucoes raras.

Detalhes em `docs/11_direcao_de_violencia_gore.md`.
