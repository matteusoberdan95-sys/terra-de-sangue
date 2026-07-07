# Registro de Decisoes

## 2026-07-07 - Padrao visual aprovado da Aldeia

Decisao: o padrao da Aldeia em Cinzas e usar poucos assets bem compostos: `aldeia_mid.png` como imagem-mestra opaca, `aldeia_fg.png` como primeiro plano transparente, alinhamento em `AldeiaBackground.tscn` e vida ambiente leve em `AldeiaParallaxBackground.cs`.

Motivo: esse fluxo eliminou linhas de debug/emendas, evitou personagem andando no ceu e manteve a qualidade da pintura original. O usuario aprovou explicitamente esse resultado como o padrao a seguir.

Regra: Cursor/Codex nao devem voltar a empilhar PNGs opacos, recortar faixas com `RegionRect` ou recriar fundo procedural em C#.

## 2026-07-07 - Cenario Aldeia: cena Godot, nao codigo

Decisao: fundo da Aldeia montado em `scenes/levels/AldeiaBackground.tscn`; C# apenas parallax horizontal.

Motivo: PNGs opacos da IA nao funcionam empilhados/recortados em codigo; ajustes visuais devem ser feitos no editor pelo usuario, nao via constantes no agente.

## 2026-07-07 - Imagem-mestra + FG

Decisao: usar `aldeia_mid.png` como cenario completo e `aldeia_fg.png` como primeiro plano opcional ate existirem camadas com transparencia real.

Motivo: qualidade visual estavel sem emendas; `aldeia_ground` atual e vista de cima e foi afastado do runtime.

## 2026-07-07 - Commit e push antes da proxima sprint

Decisao: toda sprint validada precisa ser commitada e enviada ao GitHub antes de iniciar a proxima sprint.

Motivo: manter o mesmo rigor da validacao visual e evitar perda de contexto entre Codex, Cursor e o repositorio remoto.

## 2026-07-07 - Nome do jogo

Decisao: usar `Terra Sangrada` como nome de producao.

Motivo: comunica territorio, violencia, luto e profanacao.

## 2026-07-07 - Processo por sprints

Decisao: cada sprint precisa terminar com validacao visual no Godot antes da proxima comecar.

Motivo: evitar repetir o problema do projeto anterior, onde muitas cenas foram criadas antes do polimento do nucleo.

## 2026-07-07 - Cultura ficticia

Decisao: o povo, simbolos e rituais do jogo serao ficticios ate haver pesquisa dedicada.

Motivo: permitir liberdade artistica sem tratar culturas indigenas reais de forma rasa.

## 2026-07-07 - Agentes com nomes de Dragon Ball Z

Decisao: usar apelidos de Dragon Ball Z para agentes de validacao.

Motivo: facilitar memoria operacional e dividir responsabilidades de qualidade.

## 2026-07-07 - Primeiro foco tecnico

Decisao: provar combate com placeholders antes de produzir arte final.

Motivo: se o combate nao for bom com formas simples, arte final nao resolve o problema central.

## 2026-07-07 - Identidade gore adulta

Decisao: Terra Sangrada sera explicitamente 18+, sangrento e brutal, com dano visual progressivo, sangue no ambiente, roupas rasgadas, mutilacoes, cabecas cortadas, barrigas abertas, visceras, membros decepados e execucoes contextuais por arma.

Motivo: tornar o jogo unico dentro do beat 'em up 2D e reforcar o tom dark, violento e melancolico do projeto.
